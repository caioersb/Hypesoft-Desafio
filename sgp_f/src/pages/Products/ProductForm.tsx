import { useEffect } from "react";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

export type CategoryOption = { id: string; name: string };

export const productSchema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string().min(1, "Description is required"),
  price: z
    .string()
    .refine((v) => !Number.isNaN(Number(v)) && Number(v) > 0, {
      message: "Price must be a number greater than 0",
    }),
  categoryId: z.string().min(1, "Category is required"),
  stockQuantity: z
    .string()
    .refine((v) => /^\d+$/.test(v) && Number(v) >= 0, {
      message: "Stock must be a non-negative integer",
    }),
});
export type ProductFormValues = z.infer<typeof productSchema>;

export function ProductForm({
  defaultValues,
  onSubmit,
  submitting,
  title = "Product",
  submitLabel = "Save",
  onDelete,
  categories,
}: {
  defaultValues?: Partial<ProductFormValues>;
  onSubmit: (values: ProductFormValues) => void;
  submitting?: boolean;
  title?: string;
  submitLabel?: string;
  onDelete?: () => void;
  categories: CategoryOption[];
}) {
  const form = useForm<ProductFormValues>({
    resolver: zodResolver(productSchema),
    defaultValues: {
      name: "",
      description: "",
      price: "",
      categoryId: "",
      stockQuantity: "",
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
        price: defaultValues.price ?? "",
        categoryId: defaultValues.categoryId ?? "",
        stockQuantity: defaultValues.stockQuantity ?? "",
      });
    }
  }, [defaultValues, form]);

  // When categories load, ensure the select shows the intended default category
  useEffect(() => {
    const current = form.getValues("categoryId");
    const desired = defaultValues?.categoryId;
    if (desired && (!current || current !== desired)) {
      const exists = categories.some((c) => c.id === desired);
      if (exists) {
        form.setValue("categoryId", desired, { shouldDirty: false, shouldValidate: true });
      }
    }
  }, [categories, defaultValues?.categoryId, form]);

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
                    <FormLabel>Name</FormLabel>
                    <FormControl>
                      <Input placeholder="Product name" {...field} />
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
                    <FormLabel>Description</FormLabel>
                    <FormControl>
                      <Input placeholder="Short description" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="price"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Price</FormLabel>
                    <FormControl>
                      <Input inputMode="decimal" placeholder="0.00" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="categoryId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Category</FormLabel>
                    <FormControl>
                      <Select value={field.value} onValueChange={field.onChange}>
                        <SelectTrigger>
                          <SelectValue placeholder="Select a category" />
                        </SelectTrigger>
                        <SelectContent>
                          {categories.map((c) => (
                            <SelectItem key={c.id} value={c.id}>{c.name}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="stockQuantity"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Quantity in stock</FormLabel>
                    <FormControl>
                      <Input inputMode="numeric" pattern="[0-9]*" placeholder="0" {...field} />
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
