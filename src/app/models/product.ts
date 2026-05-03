export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  stock: number;
}

export interface ProductDTO {
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  stock: number;
}