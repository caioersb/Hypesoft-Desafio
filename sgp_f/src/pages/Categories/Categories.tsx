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
import { categoriesService } from "@/services/categories";
import { toast } from "@/hooks/use-toast";

type CategoryRow = {
  id: string;
  name: string;
  description: string;
};

export default function Categories() {
  const [query, setQuery] = useState("");
  const [items, setItems] = useState<CategoryRow[]>([]);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data: any[] = await categoriesService.listAll<any[]>();
        if (!mounted) return;
        const rows: CategoryRow[] = data.map((c: any) => ({
          id: String(c.id),
          name: c.name,
          description: c.description ?? "",
        }));
        setItems(rows);
      } catch (e: any) {
        toast({ title: "Failed to load categories", description: e?.message ?? "" });
      }
    })();
    return () => { mounted = false; };
  }, []);

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return items;
    return items.filter((p) => p.name.toLowerCase().includes(q));
  }, [query, items]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-2">
          <h2 className="text-xl font-semibold">Categories</h2>
          <p className="text-sm text-muted-foreground">
            Manage your categories here.
          </p>
        </div>
        <Button asChild>
          <Link to="/categories/new">Add new category</Link>
        </Button>
      </div>

      <div className="flex flex-col">
        <Input
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search by category name..."
          className="max-w-md border border-input border-gray-100 rounded"
        />
      </div>

      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-sm font-medium">All categories</CardTitle>
        </CardHeader>
        <CardContent className="pt-2">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Description</TableHead>
                <TableHead className="w-[100px]"></TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {filtered.map((p) => (
                <TableRow key={p.id}>
                  <TableCell className="font-medium">{p.name}</TableCell>
                  <TableCell>{p.description}</TableCell>
                  <TableCell className="text-right">
                    <Button asChild size="sm" variant="outline">
                      <Link to={`/categories/${p.id}`}>Edit</Link>
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
              {!filtered.length && (
                <TableRow>
                  <TableCell
                    colSpan={2}
                    className="h-24 text-center text-muted-foreground">
                    No categories found.
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
