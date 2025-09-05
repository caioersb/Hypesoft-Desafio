import { useEffect, useMemo, useState } from "react";
import { DashboardChart } from "@/components/charts/DashboardChart";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { productsService } from "@/services/products";
import { toast } from "@/hooks/use-toast";

export default function Dashboard() {
  const [loading, setLoading] = useState(true);
  const [totalUnits, setTotalUnits] = useState(0);
  const [totalValue, setTotalValue] = useState(0);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const products: any[] = await productsService.listAll<any[]>();
        if (!mounted) return;
        let units = 0;
        let value = 0;
        for (const p of products) {
          const qty = Number(p.stockQuantity ?? p.stock ?? 0);
          const price = Number(p.price ?? 0);
          units += qty;
          value += qty * price;
        }
        setTotalUnits(units);
        setTotalValue(value);
      } catch (e: any) {
        toast({ title: "Failed to load dashboard data", description: e?.message ?? "" });
      } finally {
        setLoading(false);
      }
    })();
    return () => {
      mounted = false;
    };
  }, []);

  const currency = useMemo(
    () => new Intl.NumberFormat(undefined, { style: "currency", currency: "USD" }),
    []
  );

  return (
    <div className="space-y-6">
      <div className="space-y-2">
        <h2 className="text-xl font-semibold">Dashboard</h2>
        <p className="text-sm text-muted-foreground"> 
          Products overview and metrics.
        </p>
      </div>

      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-2">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">
              Total products in the system
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{loading ? "…" : totalUnits}</div>
            <p className="text-xs text-muted-foreground">{loading ? "Loading" : "Units in stock"}</p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">
              Total value of the Stock ($)
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{loading ? "…" : currency.format(totalValue)}</div>
            <p className="text-xs text-muted-foreground">{loading ? "Loading" : ""}</p>
          </CardContent>
        </Card>

      </div>

      <div className="grid gap-4">
        <Card>
          <CardHeader className="pb-2">
            <CardTitle className="text-sm font-medium">
              Products by categories
            </CardTitle>
          </CardHeader>
          <CardContent className="pt-4">
            <DashboardChart />
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
