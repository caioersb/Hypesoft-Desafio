import api from './api';

const BASE_PATH = '/product';

export type Id = string | number;

export const productsService = {
  // GET url/products/all
  async listAll<T = any>(): Promise<T> {
    const { data } = await api.get<T>(`${BASE_PATH}/all`);
    return data;
  },

  // GET url/products/:id
  async listById<T = any>(id: Id): Promise<T> {
    const { data } = await api.get<T>(`${BASE_PATH}/${id}`);
    return data;
  },

  // POST url/products
  async add<T = any, B = any>(body: B): Promise<T> {
    const dto: any = {
      name: (body as any).name,
      description: (body as any).description,
      price: (body as any).price,
      stockQuantity: (body as any).stockQuantity ?? (body as any).stock,
      categoryId: (body as any).categoryId,
    };
    const { data } = await api.post<T>(`${BASE_PATH}`, { dto });
    return data;
  },

  // PUT url/products/:id
  async edit<T = any, B = any>(id: Id, body: B): Promise<T> {
    const dto: any = {
      id,
      name: (body as any).name,
      description: (body as any).description,
      price: (body as any).price,
      stockQuantity: (body as any).stockQuantity ?? (body as any).stock,
      categoryId: (body as any).categoryId,
    };
    const { data } = await api.put<T>(`${BASE_PATH}/${id}`, { dto });
    return data;
  },

  // DELETE url/products/:id
  async remove<T = any>(id: Id): Promise<T> {
    const { data } = await api.delete<T>(`${BASE_PATH}/${id}`);
    return data;
  },

  // ...
  adjustStock: async (id: string, delta: number): Promise<number> => {
    const res = await api.post(`/Product/${id}/adjust-stock`, { delta });
    return res.data; 
  },

};

export default productsService;
