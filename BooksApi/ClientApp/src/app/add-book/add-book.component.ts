import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http'
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AddBookComponent implements OnInit {
  book = {};

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
  }

  createBook() {
    // const httpOptions = {
    //   headers: new HttpHeaders({
    //     'X-XSRF-Token': $('input[name=__RequestVerificationToken]').val()
    //   })
    // };

    //this.http.post('/api/books', this.book, httpOptions)
    this.http.post('/api/books', this.book)
      .subscribe(res => {
        let id = res['Id'];
        this.router.navigate(['/details', id]);
      }, (err) => {
        console.log(err);
      });
  }
}
