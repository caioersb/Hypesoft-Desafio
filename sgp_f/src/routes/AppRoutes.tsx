import { Routes, Route } from "react-router-dom";
import { AppLayout } from "@/components/layout/AppLayout";
import Dashboard from "@/pages/Dashboard";
import Products from "@/pages/Products/Products";
import Categories from "@/pages/Categories/Categories";
import StockControl from "@/pages/Stock/StockControl";
import NewProduct from "@/pages/Products/NewProduct";
import EditProduct from "@/pages/Products/EditProduct";
import NewCategory from "@/pages/Categories/NewCategory";
import EditCategory from "@/pages/Categories/EditCategory";
import { ProtectedRoute } from "@/routes/ProtectedRoute";

export function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<AppLayout />}>
        <Route
          index
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="products"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <Products />
            </ProtectedRoute>
          }
        />
        <Route
          path="products/new"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <NewProduct />
            </ProtectedRoute>
          }
        />
        <Route
          path="products/:id"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <EditProduct />
            </ProtectedRoute>
          }
        />
        <Route
          path="categories"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <Categories />
            </ProtectedRoute>
          }
        />
        <Route
          path="categories/new"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <NewCategory />
            </ProtectedRoute>
          }
        />
        <Route
          path="categories/:id"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <EditCategory />
            </ProtectedRoute>
          }
        />
        <Route
          path="stock"
          element={
            <ProtectedRoute requiredRoles={["manager", "admin"]}>
              <StockControl />
            </ProtectedRoute>
          }
        />
      </Route>
    </Routes>
  );
}
