import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { CategoryForm, CategoryFormValues } from "./CategoryForm";
import { toast } from "@/hooks/use-toast";
import { categoriesService } from "@/services/categories";

export default function NewCategory() {
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(values: CategoryFormValues) {
    setSubmitting(true);
    try {
      const payload = { name: values.name, description: values.description };
      await categoriesService.add(payload);
      toast({ title: "Category created", description: values.name });
      navigate("/categories");
    } catch (e: any) {
      toast({ title: "Error creating category", description: e?.message ?? "" });
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <CategoryForm
      title="New Category"
      submitLabel="Create"
      onSubmit={handleSubmit}
      submitting={submitting}
    />
  );
}
