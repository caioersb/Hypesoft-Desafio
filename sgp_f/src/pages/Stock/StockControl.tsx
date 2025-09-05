import { useEffect, useMemo, useState } from "react";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { productsService } from "@/services/products";
import { categoriesService } from "@/services/categories";

export type Product = {
  id: string;
  name: string;
  description: string;
  category: string;
  price: number;
  stock: number;
};

export default function StockControl() {
  const [products, setProducts] = useState<Product[]>([]);
  const [categories, setCategories] = useState<{ id: string; name: string }[]>([]);
  const [query, setQuery] = useState("");
  const [lowOnly, setLowOnly] = useState(false);
  const [pendingValues, setPendingValues] = useState<Record<string, string>>({});

  useEffect(() => {
    (async () => {
      try {
        const categoriesData: any[] = await categoriesService.listAll<any[]>();
        setCategories(
          categoriesData.map((c: any) => ({ id: String(c.id), name: c.name }))
        );

        const data: any[] = await productsService.listAll<any[]>();
        setProducts(
          data.map((p: any) => {
            const categoryObj = categoriesData.find(
              (c: any) => String(c.id) === String(p.categoryId)
            );
            return {
              id: String(p.id),
              name: p.name,
              description: p.description,
              category: categoryObj?.name ?? "Uncategorized",
              price: Number(p.price ?? 0),
              stock: Number(p.stockQuantity ?? 0),
            };
          })
        );
      } catch (e) {
        console.error("Failed to load products/categories", e);
      }
    })();
  }, []);

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    let list = !q
      ? products
      : products.filter((p) => p.name.toLowerCase().includes(q));
    if (lowOnly) list = list.filter((p) => p.stock <= 10);
    return list;
  }, [query, products, lowOnly]);

  async function adjustStock(id: string, delta: number) {
    try {
      const newQty = await productsService.adjustStock(id, delta);
      setProducts((prev) =>
        prev.map((p) => (p.id === id ? { ...p, stock: newQty } : p))
      );
    } catch (e) {
      console.error("Adjust stock failed", e);
    }
  }

  function onPendingChange(id: string, v: string) {
    setPendingValues((prev) => ({ ...prev, [id]: v }));
  }

  async function applyPending(id: string) {
    const raw = pendingValues[id];
    if (raw == null || raw === "") return;
    const parsed = Number(raw);
    if (!Number.isFinite(parsed)) return;

    const current = products.find((p) => p.id === id)?.stock ?? 0;
    const delta = parsed - current;
    if (delta === 0) return;

    await adjustStock(id, delta);
    setPendingValues((prev) => ({ ...prev, [id]: "" }));
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-2">
          <h2 className="text-xl font-semibold">Stock Control</h2>
          <p className="text-sm text-muted-foreground">
            Manage product quantities, adjust stock manually, and spot low inventory.
          </p>
        </div>
      </div>

      <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
        <Input
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search by product name..."
          className="max-w-md border border-input border-gray-100 rounded"
        />
        <label className="flex items-center gap-2 text-sm">
          <input
            type="checkbox"
            checked={lowOnly}
            onChange={(e) => setLowOnly(e.target.checked)}
          />
          Show only low stock (≤ 10)
        </label>
      </div>

      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-sm font-medium">Inventory</CardTitle>
        </CardHeader>
        <CardContent className="pt-2">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Category</TableHead>
                <TableHead className="text-right">Current Stock</TableHead>
                <TableHead className="w-[260px] text-right">Adjust</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filtered.map((p) => {
                const low = p.stock <= 10;
                return (
                  <TableRow key={p.id}>
                    <TableCell className="font-medium flex items-center gap-2">
                      {p.name}
                      {low && (
                        <Badge variant="destructive" className="ml-1">
                          Low
                        </Badge>
                      )}
                    </TableCell>
                    <TableCell>{p.category}</TableCell>
                    <TableCell className="text-right">{p.stock}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={async () => await adjustStock(p.id, -1)}
                        >
                          -1
                        </Button>
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={async () => await adjustStock(p.id, +1)}
                        >
                          +1
                        </Button>
                        <Input
                          inputMode="numeric"
                          pattern="[0-9]*"
                          placeholder="Set qty"
                          className="w-24 h-8"
                          value={pendingValues[p.id] ?? ""}
                          onChange={(e) => onPendingChange(p.id, e.target.value)}
                        />
                        <Button size="sm" onClick={async () => await applyPending(p.id)}>
                          Update
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                );
              })}
              {!filtered.length && (
                <TableRow>
                  <TableCell
                    colSpan={4}
                    className="h-24 text-center text-muted-foreground"
                  >
                    No products to show.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  );
}
