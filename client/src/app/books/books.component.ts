import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Apollo } from 'apollo-angular';
import { GET_BOOKS, PUBLISH_BOOK } from '../queries/book.queries';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss']
})
export class BooksComponent implements OnInit {
  books: any[] = [];

  bookForm = new FormGroup({
    id: new FormControl('', Validators.required),
    title: new FormControl('', Validators.required),
    author: new FormControl('', Validators.required)
  });

  constructor(private apollo: Apollo) { }

  addBook() {
    this.apollo.mutate({
      mutation: PUBLISH_BOOK,
      variables: {
        input: {
          id: this.bookForm.value.id,
          title: this.bookForm.value.title,
          authorName: this.bookForm.value.author
        }
      },
      refetchQueries: [{
        query: GET_BOOKS
      }]
    }).subscribe({
      next: ({ data }: any) => {
        this.books = data.books;
        this.bookForm.reset();
      }, error: (error) => {
        console.error(error);
      }
    });
  }

  ngOnInit(): void {
    this.apollo.watchQuery<any>({
      query: GET_BOOKS
    }).valueChanges.subscribe({
      next: ({ data }) => {
        this.books = data.books;
      }, error: (error) => {
        console.error(error);
      }
    });
  }

}
