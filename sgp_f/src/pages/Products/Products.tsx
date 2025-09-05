import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";

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
import { productsService } from "@/services/products";
import { categoriesService } from "@/services/categories"; // Add this import
import { toast } from "@/hooks/use-toast";

type ProductRow = {
  id: string;
  name: string;
  description: string;
  category: string;
  price: number;
  stockQuantity: number;
};

type Category = {
  id: string;
  name: string;
};

export default function Products() {
  const [query, setQuery] = useState("");
  const [items, setItems] = useState<ProductRow[]>([]);
  const [categories, setCategories] = useState<Category[]>([]); // Add categories state

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        // Fetch categories
        const categoriesData: any[] = await categoriesService.listAll<any[]>();
        if (!mounted) return;
        setCategories(categoriesData.map((c: any) => ({
          id: String(c.id),
          name: c.name,
        })));

        // Fetch products
        const data: any[] = await productsService.listAll<any[]>();
        if (!mounted) return;
        const rows: ProductRow[] = data.map((p: any) => {
          const categoryObj = categoriesData.find((c: any) => String(c.id) === String(p.categoryId));
          return {
            id: String(p.id),
            name: p.name,
            description: p.description,
            category: categoryObj?.name ?? "Uncategorized",
            price: Number(p.price ?? 0),
            stockQuantity: Number(p.stockQuantity ?? 0),
          };
        });
        setItems(rows);
      } catch (e: any) {
        toast({ title: "Failed to load products", description: e?.message ?? "" });
      }
    })();
    return () => {
      mounted = false;
    };
  }, []);

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return items;
    return items.filter((p) => p.name.toLowerCase().includes(q));
  }, [query, items]);

  const currency = useMemo(
    () => new Intl.NumberFormat(undefined, { style: "currency", currency: "USD" }),
    []
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-2">
          <h2 className="text-xl font-semibold">Products</h2>
          <p className="text-sm text-muted-foreground">
            Manage your product catalog here.
          </p>
        </div>
        <Button asChild>
          <Link to="/products/new">Add new product</Link>
        </Button>
      </div>

      <div className="flex flex-col">
        <Input
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search by product name..."
          className="max-w-md border border-input border-gray-100 rounded"
        />
      </div>

      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-sm font-medium">All products</CardTitle>
        </CardHeader>
        <CardContent className="pt-2">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Description</TableHead>
                <TableHead>Category</TableHead>
                <TableHead className="text-right">Price</TableHead>
                <TableHead className="text-right">Stock</TableHead>
                <TableHead className="w-[100px]"></TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filtered.map((p) => (
                <TableRow key={p.id}>
                  <TableCell className="font-medium">{p.name}</TableCell>
                  <TableCell>{p.description}</TableCell>
                  <TableCell>{p.category}</TableCell>
                  <TableCell className="text-right">
                    {currency.format(p.price)}
                  </TableCell>
                  <TableCell className="text-right">{p.stockQuantity}</TableCell>
                  <TableCell className="text-right">
                    <Button asChild size="sm" variant="outline">
                      <Link to={`/products/${p.id}`}>Edit</Link>
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
              {!filtered.length && (
                <TableRow>
                  <TableCell
                    colSpan={6}
                    className="h-24 text-center text-muted-foreground">
                    No products found.
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