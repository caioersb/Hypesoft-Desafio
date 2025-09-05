import { ReactNode } from "react";
import { AppSidebar } from "@/components/layout/AppSidebar";
import { SidebarInset } from "@/components/ui/sidebar";
import { SidebarTrigger } from "@/components/ui/sidebar";
import { Outlet } from "react-router-dom";

export function AppLayout({ children }: { children?: ReactNode }) {
  return (
    <>
      <AppSidebar />
      <SidebarInset>
        <header className="flex items-center gap-2 border-b px-4 py-2">
          <SidebarTrigger />
          <h1 className="text-sm font-medium">Products Management</h1>
        </header>
        <main className="p-4">
          {children ?? <Outlet />}
        </main>
      </SidebarInset>
    </>
  );
}
