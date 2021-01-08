
function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';
    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState)
                {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    


    self.callApi = function () {
        ShowLoad();
        self.result('');
        self.errors.removeAll();
        

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/values',
            headers: headers
        }).done(function (data) {
            self.result(data);
            console.log(data);
            HideLoad();
        }).fail(showError);
        HideLoad();
            
        
    }

    self.register = function () {
        grecaptcha.ready(function () {
            grecaptcha.execute('6LeQZBsaAAAAAOfqEjOH53PZepwuxcJdFqJKd4gL').then(function (token1) {
                self.result('');
                self.errors.removeAll();
                ShowLoad();
                var data = {
                    Email: self.registerEmail(),
                    Password: self.registerPassword(),
                    ConfirmPassword: self.registerPassword2()
                };

                $.ajax({
                    type: 'POST',
                    url: '/api/Account/Register',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(data)
                }).done(function (data) {
                    self.result("Done!");
                    HideLoad();
                }).fail(showError);
                    HideLoad();
                

                console.log(token1);
            });
        });
        
    }

    self.login = function () {
        self.result('');
        self.errors.removeAll();
        
        ShowLoad(); 
        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
            HideLoad();
        }).fail(showError);
        HideLoad();
        
    }

    self.logout = function () {
        // Log out from the cookie based logon.
        ShowLoad();
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/Account/Logout'
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            self.user('');
            sessionStorage.removeItem(tokenKey);
            HideLoad();
        }).fail(showError); 
                HideLoad();
            
    }
}

function ShowLoad() {
    console.log("loading show");
    document.getElementById('container').hidden = true;
    document.getElementById('loading1').hidden = false;
}


function HideLoad() {
    console.log("loading hide");
    document.getElementById('container').hidden = false;
    document.getElementById('loading1').hidden = true;
}

var app = new ViewModel();

ko.applyBindings(app);