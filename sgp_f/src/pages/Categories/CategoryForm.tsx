import { useEffect } from "react";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form";

export const categorySchema = z.object({
  name: z.string().min(1, "Title is required"),
  description: z.string().optional(),
});
export type CategoryFormValues = z.infer<typeof categorySchema>;

export function CategoryForm({
  defaultValues,
  onSubmit,
  submitting,
  title = "Category",
  submitLabel = "Save",
  onDelete,
}: {
  defaultValues?: Partial<CategoryFormValues>;
  onSubmit: (values: CategoryFormValues) => void;
  submitting?: boolean;
  title?: string;
  submitLabel?: string;
  onDelete?: () => void;
}) {
  const form = useForm<CategoryFormValues>({
    resolver: zodResolver(categorySchema),
    defaultValues: {
      name: "",
      description: "",
      ...defaultValues,
    },
    mode: "onChange",
  });

  // When defaultValues change (e.g., after async fetch), update the form fields
  useEffect(() => {
    if (defaultValues) {
      form.reset({
        name: defaultValues.name ?? "",
        description: defaultValues.description ?? "",
      });
    }
  }, [defaultValues, form]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-2">
          <h2 className="text-xl font-semibold">{title}</h2>
          <p className="text-sm text-muted-foreground">Fill in the fields below. Validation happens in real time.</p>
        </div>
        {onDelete && (
          <Button variant="destructive" onClick={onDelete}>Delete</Button>
        )}
      </div>

      <Card>
        <CardHeader className="pb-2">
          <CardTitle className="text-sm font-medium">Details</CardTitle>
        </CardHeader>
        <CardContent className="pt-2 space-y-4">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 max-w-xl">
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Title</FormLabel>
                    <FormControl>
                      <Input placeholder="Category title" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Description (optional)</FormLabel>
                    <FormControl>
                      <Input placeholder="Short description" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <div className="flex gap-2">
                <Button type="submit" disabled={submitting || !form.formState.isValid}>{submitLabel}</Button>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
