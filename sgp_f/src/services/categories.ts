import api from './api';

const BASE_PATH = '/category';

export type Id = string | number;

export const categoriesService = {
  // GET url/categories
  async listAll<T = any>(): Promise<T> {
    const { data } = await api.get<T>(`${BASE_PATH}`);
    return data;
  },

  // GET url/categories/:id
  async listById<T = any>(id: Id): Promise<T> {
    const { data } = await api.get<T>(`${BASE_PATH}/${id}`);
    return data;
  },

  // POST url/categories
  async add<T = any, B = any>(body: B): Promise<T> {
    const dto: any = {
      name: (body as any).name,
      description: (body as any).description,
    };
    const { data } = await api.post<T>(`${BASE_PATH}`, { dto });
    return data;
  },

  // PUT url/categories/:id
  async edit<T = any, B = any>(id: Id, body: B): Promise<T> {
    const dto: any = {
      id,
      name: (body as any).name,
      description: (body as any).description,
    };
    const { data } = await api.put<T>(`${BASE_PATH}/${id}`, { dto });
    return data;
  },

  // DELETE url/categories/:id
  async remove<T = any>(id: Id): Promise<T> {
    const { data } = await api.delete<T>(`${BASE_PATH}/${id}`);
    return data;
  },
};

export default categoriesService;
