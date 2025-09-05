import { NavLink } from "react-router-dom";
import {
  Sidebar,
  SidebarContent,
  SidebarHeader,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuItem,
  SidebarMenuButton,
  SidebarFooter,
  SidebarSeparator,
} from "@/components/ui/sidebar";
import { cn } from "@/lib/utils";
import { LayoutDashboard, Box, Tag, Package, LogOut } from "lucide-react";
import logoUrl from "@/assets/logo.svg";
import { useAuth } from "@/providers/AuthProvider";

export function AppSidebar() {
  const { keycloak, logout } = useAuth();
  const username = keycloak?.tokenParsed?.preferred_username ?? "Guest";

  return (
    <Sidebar>
      <SidebarHeader>
        <div className="flex items-center gap-2 p-2">
          <img src={logoUrl} alt="Logo" className="h-7 w-7" />
          <span className="font-semibold text-[20px]">Products Admin</span>
        </div>
      </SidebarHeader>

      <SidebarSeparator />

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel className="text-[18px]">Management</SidebarGroupLabel>

          <SidebarMenu className="mt-3 gap-2">
            <SidebarMenuItem>
              <SidebarMenuButton asChild isActive={false}>
                <NavLink
                  to="/"
                  className={({ isActive }) =>
                    cn("flex items-center gap-2", isActive && "data-[active=true]")
                  }
                >
                  <LayoutDashboard className="size-4" />
                  <span className="text-[16px]">Dashboard</span>
                </NavLink>
              </SidebarMenuButton>
            </SidebarMenuItem>

            <SidebarMenuItem>
              <SidebarMenuButton asChild isActive={false}>
                <NavLink
                  to="/stock"
                  className={({ isActive }) =>
                    cn("flex items-center gap-2", isActive && "data-[active=true]")
                  }
                >
                  <Package className="size-4" />
                  <span className="text-[16px]">Stock Control</span>
                </NavLink>
              </SidebarMenuButton>
            </SidebarMenuItem>

            <SidebarMenuItem>
              <SidebarMenuButton asChild isActive={false}>
                <NavLink
                  to="/products"
                  className={({ isActive }) =>
                    cn("flex items-center gap-2", isActive && "data-[active=true]")
                  }
                >
                  <Box className="size-4" />
                  <span className="text-[16px]">Products</span>
                </NavLink>
              </SidebarMenuButton>
            </SidebarMenuItem>

            <SidebarMenuItem>
              <SidebarMenuButton asChild isActive={false}>
                <NavLink
                  to="/categories"
                  className={({ isActive }) =>
                    cn("flex items-center gap-2", isActive && "data-[active=true]")
                  }
                >
                  <Tag className="size-4" />
                  <span className="text-[16px]">Categories</span>
                </NavLink>
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter>
        <div className="flex flex-col gap-2 px-2">
          <div className="text-sm font-medium">{username}</div>
          <button
            onClick={logout}
            className="flex items-center gap-1 text-red-500 hover:text-red-700"
          >
            <LogOut className="size-4" />
            Logout
          </button>
          <div className="text-xs text-muted-foreground">v0.1.0</div>
        </div>
      </SidebarFooter>
    </Sidebar>
  );
}
