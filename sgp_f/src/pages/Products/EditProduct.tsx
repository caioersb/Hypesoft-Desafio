import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ProductForm, ProductFormValues, type CategoryOption } from "./ProductForm";
import { toast } from "@/hooks/use-toast";
import { productsService } from "@/services/products";
import { categoriesService } from "@/services/categories";

export default function EditProduct() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [categories, setCategories] = useState<CategoryOption[]>([]);
  const [defaults, setDefaults] = useState<Partial<ProductFormValues>>();
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (!id) return;
    let mounted = true;
    (async () => {
      try {
        const [product, cats] = await Promise.all([
          productsService.listById<any>(id),
          categoriesService.listAll<any[]>(),
        ]);
        if (!mounted) return;
        const options: CategoryOption[] = cats.map((c: any) => ({ id: String(c.id), name: c.name }));
        setCategories(options);
        const categoryName = (product.category?.name ?? product.categoryName) as string | undefined;
        const matched = categoryName ? options.find((o) => o.name === categoryName) : undefined;
        const resolvedCategoryId = String(
          product.categoryId ?? product.category?.id ?? matched?.id ?? ""
        );
        setDefaults({
          name: product.name ?? "",
          description: product.description ?? "",
          price: String(product.price ?? ""),
          stockQuantity: String((product.stockQuantity ?? product.stockQuantity) ?? ""),
          categoryId: resolvedCategoryId,
        });
      } catch (e: any) {
        toast({ title: "Failed to load product", description: e?.message ?? "" });
      }
    })();
    return () => {
      mounted = false;
    };
  }, [id]);

  async function handleSubmit(values: ProductFormValues) {
    if (!id) return;
    setSubmitting(true);
    try {
      const payload = {
        name: values.name,
        description: values.description,
        price: Number(values.price),
        stockQuantity: Number(values.stockQuantity),
        categoryId: values.categoryId,
      };
      await productsService.edit(id, payload);
      toast({ title: "Product updated", description: values.name });
      navigate("/products");
    } catch (e: any) {
      toast({ title: "Error updating product", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete() {
    if (!id) return;
    setSubmitting(true);
    try {
      await productsService.remove(id);
      toast({ title: "Product deleted", description: id });
      navigate("/products");
    } catch (e: any) {
      toast({ title: "Error deleting product", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <ProductForm
      title="Edit Product"
      submitLabel="Save changes"
      defaultValues={defaults}
      onSubmit={handleSubmit}
      onDelete={handleDelete}
      submitting={submitting}
      categories={categories}
    />
  );
}
