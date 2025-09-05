import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ProductForm, ProductFormValues, type CategoryOption } from "./ProductForm";
import { toast } from "@/hooks/use-toast";
import { productsService } from "@/services/products";
import { categoriesService } from "@/services/categories";

export default function NewProduct() {
  const navigate = useNavigate();
  const [categories, setCategories] = useState<CategoryOption[]>([]);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data: any[] = await categoriesService.listAll<any[]>();
        if (!mounted) return;
        const options: CategoryOption[] = data.map((c: any) => ({ id: String(c.id), name: c.name }));
        setCategories(options);
      } catch (e: any) {
        toast({ title: "Failed to load categories", description: e?.message ?? "" });
      }
    })();
    return () => {
      mounted = false;
    };
  }, []);

  async function handleSubmit(values: ProductFormValues) {
    setSubmitting(true);
    try {
      const payload = {
        name: values.name,
        description: values.description,
        price: Number(values.price),
        categoryId: values.categoryId,
        stock: Number(values.stockQuantity),
      };
      await productsService.add(payload);
      toast({ title: "Product created", description: values.name });
      navigate("/products");
    } catch (e: any) {
      toast({ title: "Error creating product", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <ProductForm
      title="New Product"
      submitLabel="Create"
      onSubmit={handleSubmit}
      submitting={submitting}
      categories={categories}
    />
  );
}
