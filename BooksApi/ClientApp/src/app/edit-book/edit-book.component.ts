import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class EditBookComponent implements OnInit {
  book = {};
  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getBookDetail(this.route.snapshot.params['id']);
  }

  getBookDetail(id) {
    this.http.get('/api/books/' + id).subscribe(data => {
      this.book = data;
    });
  }
  updateBook(id) {
    this.http.put('/api/books/' + id, this.book)
      .subscribe(res => {
        let id = res['Id'];
        this.router.navigate(['/details', id]);
      }, (err) => {
        console.log(err);
      }
      );
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
