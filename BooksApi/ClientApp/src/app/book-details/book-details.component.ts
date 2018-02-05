import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class BookDetailsComponent implements OnInit {
  book = {};
  constructor(private router: Router, private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit() {
    this.getBookDetail(this.route.snapshot.params['id']);
  }

  getBookDetail(id) {
    this.http.get('/api/books/' + id).subscribe(data => {
      this.book = data;
    });
  }

  deleteBook(id) {
    this.http.delete('/api/books/' + id)
      .subscribe(res => {
        this.router.navigate(['/books']);
      }, (err) => {
        console.log(err);
      }
      );
  }

}
