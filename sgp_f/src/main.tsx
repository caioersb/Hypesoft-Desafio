import { createRoot } from 'react-dom/client'
import './index.css'
import { SidebarProvider } from './components/ui/sidebar'
import { ThemeProvider } from './providers/ThemeProvider'
import { AuthProvider } from './providers/AuthProvider'
import { AppRoutes } from './routes/AppRoutes'
import { BrowserRouter } from 'react-router-dom'

createRoot(document.getElementById('root')!).render(
	// <StrictMode>
		<BrowserRouter>
			<ThemeProvider>
				<SidebarProvider>
					<AuthProvider>
						<AppRoutes />
					</AuthProvider>
				</SidebarProvider>
			</ThemeProvider>
		</BrowserRouter>
	// </StrictMode>
)
