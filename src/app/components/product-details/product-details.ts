import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product';
import { CartService } from '../../services/cart';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-details.html'
})
export class ProductDetailsComponent implements OnInit {
  product: Product | null = null;
  quantity = 1;
  added = false;
  loading = true;

  constructor(
  private route: ActivatedRoute,
  public router: Router,
  private productService: ProductService,
  private cartService: CartService
) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getById(id).subscribe({
      next: p => { this.product = p; this.loading = false; },
      error: () => { this.loading = false; this.router.navigate(['/products']); }
    });
  }

  

goBack(): void {
  this.router.navigate(['/products']);
}



  increment(): void {
    if (this.product && this.quantity < this.product.stock) this.quantity++;
  }

  decrement(): void {
    if (this.quantity > 1) this.quantity--;
  }

  addToCart(): void {
    if (!this.product) return;
    for (let i = 0; i < this.quantity; i++) this.cartService.addToCart(this.product);
    this.added = true;
    setTimeout(() => this.added = false, 2000);
  }
}