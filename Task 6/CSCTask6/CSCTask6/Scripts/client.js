var stripe = Stripe('pk_test_51I5B7mLaXDcHpE2BfPtIZe7Pd8Z2m80hivBktXXcfqGdB2tOBfRrScaU96gibvijmmbo8FFLFtKGsBP8FH7Xf9MU00gYYJnwn3');
var elements = stripe.elements();

var elements = stripe.elements();
var style = {
    base: {
        color: "#32325d",
    }
};

var card = elements.create("card", { style: style });
card.mount("#card-element");

card.on('change', function (event) {
    var displayError = document.getElementById('card-errors');
    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
});

var form = document.getElementById('payment-form');

var response = fetch('/api/paysub').then(function (response) {
    console.log(response);
    return response.json();
}).then(function (res) {
    var clientSecret = res;
    form.addEventListener('submit', function (ev) {
        ev.preventDefault();
        var Cname = $("#name").val();
        stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: card,
                billing_details: {
                    name: Cname,
                }
            }
        }).then(function (result) {
            if (result.error) {
                // Show error to your customer (e.g., insufficient funds)
                console.log(result.error.message);
            } else {
                // The payment has been processed!
                if (result.paymentIntent.status === 'succeeded') {
                    alert("Successful Transaction.");
                    window.location.reload()
                    // Show a success message to your customer
                    // There's a risk of the customer closing the window before callback
                    // execution. Set up a webhook or plugin to listen for the
                    // payment_intent.succeeded event that handles any business critical
                    // post-payment actions.
                }
            }
        });
    });
    });

