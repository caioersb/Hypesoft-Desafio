import { ReactNode, createContext, useContext, useEffect, useState } from "react";
import Keycloak from "keycloak-js";

type AuthContextType = {
  keycloak: Keycloak | null;
  isAuthenticated: boolean;
  roles: string[];
  login: () => void;
  logout: () => void;
  token?: string;
  loading: boolean;
};

const AuthContext = createContext<AuthContextType>({
  keycloak: null,
  isAuthenticated: false,
  roles: [],
  login: () => {},
  logout: () => {},
  loading: true,
});

let currentKeycloak: Keycloak | null = null;

export function AuthProvider({ children }: { children: ReactNode }) {
  const [keycloak, setKeycloak] = useState<Keycloak | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [roles, setRoles] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const kc = new Keycloak({
      url: "http://localhost:8080",
      realm: "Hypesoft",
      clientId: "frontend-client",
    });

    kc.init({
      onLoad: "login-required",
      checkLoginIframe: false,
      pkceMethod: "S256",
      responseMode: "query",
    }).then(auth => {
      setKeycloak(kc);
      setIsAuthenticated(auth);
      currentKeycloak = kc;

      const realmRoles = kc.tokenParsed?.realm_access?.roles ?? [];
      const clientRoles = kc.tokenParsed?.resource_access?.["frontend-client"]?.roles ?? [];
      setRoles([...realmRoles, ...clientRoles]);

      setLoading(false);

      // Atualiza token automaticamente
      const interval = setInterval(() => {
        kc.updateToken(30).catch(() => kc.logout());
      }, 10000);

      // Limpa query params
      if (window.location.search.includes("code=")) {
        window.history.replaceState({}, document.title, window.location.pathname);
      }

      return () => clearInterval(interval);
    });
  }, []);

  const login = () => keycloak?.login();
  const logout = () => keycloak?.logout({ redirectUri: window.location.origin });

  if (loading) return <div>Loading...</div>;

  return (
    <AuthContext.Provider value={{ keycloak, isAuthenticated, roles, login, logout, token: keycloak?.token, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}

// Para usar no api.ts
export const getKeycloak = () => currentKeycloak;
