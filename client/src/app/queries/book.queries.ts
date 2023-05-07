import { gql } from 'apollo-angular';

const GET_BOOKS = gql`
    query getBooks {
        books {
            title
        }
    }
`;

const PUBLISH_BOOK = gql`
    mutation newBook($input: PublishBookInput!) {
        publishBook (input: $input) {
            book {
                title
            }
        }
    }
`;

export { GET_BOOKS, PUBLISH_BOOK };