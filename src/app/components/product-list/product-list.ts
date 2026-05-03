import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product';
import { CartService } from '../../services/cart';
import { AuthService } from '../../services/auth';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.html'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  filtered: Product[] = [];
  search = '';
  loading = true;
  addedId: number | null = null;

  constructor(
    private productService: ProductService,
    public cartService: CartService,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.productService.getAll().subscribe({
      next: data => { this.products = data; this.filtered = data; this.loading = false; },
      error: () => this.loading = false
    });
  }

  filterProducts(): void {
    const q = this.search.toLowerCase();
    this.filtered = this.products.filter(p =>
      p.name.toLowerCase().includes(q) || p.description.toLowerCase().includes(q)
    );
  }

  addToCart(product: Product): void {
    if (product.stock === 0) return;
    this.cartService.addToCart(product);
    this.addedId = product.productId;
    setTimeout(() => this.addedId = null, 1500);
  }

  goToDetails(id: number): void {
    this.router.navigate(['/products', id]);
  }

  editProduct(id: number): void {
    this.router.navigate(['/admin/products', id, 'edit']);
  }

  deleteProduct(id: number): void {
    if (!confirm('Delete this product?')) return;
    this.productService.delete(id).subscribe(() => {
      this.products = this.products.filter(p => p.productId !== id);
      this.filtered = this.filtered.filter(p => p.productId !== id);
    });
  }

  getStockClass(stock: number): string {
    if (stock === 0) return 'out';
    if (stock <= 5) return 'low';
    return '';
  }

  getStockLabel(stock: number): string {
    if (stock === 0) return 'Out of stock';
    if (stock <= 5) return `Only ${stock} left!`;
    return `${stock} in stock`;
  }
}