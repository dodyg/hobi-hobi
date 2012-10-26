module common {
    export function say() {
        alert('hello world');
    }
   
    //Make a post ajax json
    export function PostJson(url: string, json: any, done: (done: any) => void) {
         $.ajax(url, {
                data: json,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            }).done(done);
    }
}