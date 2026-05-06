import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { CartService, CartItem } from '../../services/cart';
import { OrderService } from '../../services/order';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './checkout.html'
})
export class CheckoutComponent {
  items: CartItem[] = [];
  total: number = 0;
  message = '';
  error = '';
  loading = false;

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    public router: Router
  ) {
    this.items = this.cartService.getItems();
    this.total = this.cartService.getTotal();
    if (this.items.length === 0) this.router.navigate(['/cart']);
  }

  placeOrder(): void {
    this.error = '';
    this.loading = true;
    const orderDTO = {
      items: this.items.map(i => ({
        productId: i.product.productId,
        quantity: i.quantity,
        price: i.product.price
      }))
    };
    this.orderService.createOrder(orderDTO).subscribe({
      next: (order) => {
        this.loading = false;
        this.cartService.clearCart();
        this.message = `Order #${order.orderId} placed successfully! Total: ₹${order.totalAmount}`;
        setTimeout(() => this.router.navigate(['/orders']), 2500);
      },
      error: () => { this.loading = false; this.error = 'Failed to place order. Please try again.'; }
    });
  }
}