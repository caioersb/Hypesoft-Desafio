import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CategoryForm, CategoryFormValues } from "./CategoryForm";
import { toast } from "@/hooks/use-toast";
import { categoriesService } from "@/services/categories";

export default function EditCategory() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [defaults, setDefaults] = useState<Partial<CategoryFormValues>>();
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (!id) return;
    let mounted = true;
    (async () => {
      try {
        const data = await categoriesService.listById<any>(id);
        if (!mounted) return;
        setDefaults({ name: data.name ?? "", description: data.description ?? "" });
      } catch (e: any) {
        toast({ title: "Failed to load category", description: e?.message ?? "" });
      }
    })();
    return () => { mounted = false; };
  }, [id]);

  async function handleSubmit(values: CategoryFormValues) {
    if (!id) return;
    setSubmitting(true);
    try {
      const payload = { name: values.name, description: values.description };
      await categoriesService.edit(id, payload);
      toast({ title: "Category updated", description: values.name });
      navigate("/categories");
    } catch (e: any) {
      toast({ title: "Error updating category", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete() {
    if (!id) return;
    setSubmitting(true);
    try {
      await categoriesService.remove(id);
      toast({ title: "Category deleted", description: id });
      navigate("/categories");
    } catch (e: any) {
      toast({ title: "Error deleting category", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <CategoryForm
      title={`Edit Category`}
      submitLabel="Save changes"
      defaultValues={defaults}
      onSubmit={handleSubmit}
      onDelete={handleDelete}
      submitting={submitting}
    />
  );
}
