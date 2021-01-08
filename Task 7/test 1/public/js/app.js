function OnLoad(){
    $(".loaderWrapper").fadeOut("slow");
}

button.addEventListener('click', function (event) {
    event.preventDefault();
    stripe.redirectToCheckout({
        sessionId: data
    });
});

function OnSubmit(){
    $(".loaderWrapper").fadeIn("slow");
}

function validURL (str) {
    var pattern = new RegExp (
      '^(https?:\\/\\/)?' + // protocol
      '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
      '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
      '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
      '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$',
      'i'
    ); // fragment locator
    return !!pattern.test (str);
  }
