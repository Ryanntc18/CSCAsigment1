const Clarifai = require('clarifai');
const express = require ('express');
var bodyParser = require('body-parser');
const app = express ();

// Set engine to enable html files
app.set('view engine', 'html');
app.engine('html', require('ejs').renderFile);
var jsonParser = bodyParser.json();

const logger = (req, res, next) => {
  console.log(`${req.protocol}://${req.get('host')}${req.originalUrl}`);
  next();
}

app.use(logger);

// Get static file
app.use(express.static(__dirname+'/public'));

// Thes Get function
app.get('/api/testing', (req, res) => res.json("Some text to show get works"));

app.post('/api/checkimg', jsonParser, (req, res) =>{
  
    var imagelink = req.body.link;
    console.log("image link: "+imagelink);
    var testImg = 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTHaZ-vu0hAXPpJH6LhJMoq1PFT3PaUiXGI8w&usqp=CAU'
    var msg = "This image NOT is a reciept.";
    app2.inputs.search({ input: { url: imagelink } }).then(
      
      function (response) {
       response.hits.forEach(hit => {
        if(hit.score > 0.8){
          msg = "This image is a reciept.";
          console.log("In 3rd func: "+ msgtext);
          //console.log(hit.input.data.image);
          
          return msg;
        }
      });
      console.log(msg);
      res.json(msg);
      },
      function (err) {
      console.log(err);
      }
      );
    
})

const PORT = process.env.PORT || 5000;

app.listen(PORT, () => console.log(`Server started on port ${PORT}`))


const app2 = new Clarifai.App({
apiKey: '8f3427184e5d404bb90d83b555b01f8a'
});
console.log("testing phase:");

var msgtext = "failed";



