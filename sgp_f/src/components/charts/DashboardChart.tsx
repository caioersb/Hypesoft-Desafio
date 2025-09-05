import { useEffect, useMemo, useState } from "react";
import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import { Bar, BarChart, CartesianGrid, XAxis, YAxis } from "recharts";
import { productsService } from "@/services/products";
import { categoriesService } from "@/services/categories";
import { toast } from "@/hooks/use-toast";

type ChartRow = { category: string; products: number };

export function DashboardChart() {
  const [rows, setRows] = useState<ChartRow[]>([]);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const [products, categories]: [any[], any[]] = await Promise.all([
          productsService.listAll<any[]>(),
          categoriesService.listAll<any[]>(),
        ]);
        if (!mounted) return;
        const catMap = new Map<string, string>(
          categories.map((c: any) => [String(c.id), c.name])
        );
        const counts = new Map<string, number>();
        for (const p of products) {
          const catName = catMap.get(String(p.categoryId)) ?? "Uncategorized";
          counts.set(catName, (counts.get(catName) ?? 0) + 1);
        }
        const data: ChartRow[] = Array.from(counts.entries()).map(
          ([category, products]) => ({ category, products })
        );
        setRows(data);
      } catch (e: any) {
        toast({
          title: "Failed to load chart",
          description: e?.message ?? "",
        });
      }
    })();

    return () => {
      mounted = false;
    };
  }, []);

  const categoriesData = useMemo(() => rows, [rows]);

  const chartConfig = {
    products: {
      label: "Products",
      color: "hsl(var(--chart-1))",
    },
  } as const;

  return (
    <ChartContainer config={chartConfig} className="h-72 w-full">
      <BarChart data={categoriesData}>
        <CartesianGrid vertical={false} />
        <XAxis
          dataKey="category"
          tickLine={false}
          axisLine={false}
          tickMargin={8}
        />
        <YAxis allowDecimals={false} tickLine={false} axisLine={false} />
        <ChartTooltip cursor={false} content={<ChartTooltipContent />} />
        <Bar dataKey="products" fill="var(--color-products)" radius={4} />
      </BarChart>
    </ChartContainer>
  );
}
