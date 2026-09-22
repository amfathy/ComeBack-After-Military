import express from 'express'
import mongoose from 'mongoose';
import dotenv from 'dotenv'
import dns from 'dns';
import Article from "./models/article.js";

const app = express() ;
dotenv.config();
dns.setServers(["8.8.8.8", "8.8.4.4"]);

app.use(express.json()); //if you missed this line tyou can't use the body of the request that you send. 
mongoose.connect(process.env.MONGODB_URI)
    .then(() => {
        console.log("MongoDB connected successfully");
    })
    .catch(err => {
        console.error("MongoDB connection error:", err);
    });
app.get("/hello" , (req,res)=>{
    res.send("Hello");
}); 

//use params of the request  
//http://localhost:3000/paramss/5/6
app.get("/paramss/:number1/:number2" , (req , res)=>{
    const n1 = req.params.number1;
    const n2 = req.params.number2;
    const total = Number(n1) + Number(n2);
    res.send(`The total is ${total}`);

})

//use body paramter 
//http://localhost:3000/boo
/*
raw : 
{ 
    "name" : "ahmed" ,
    "age" : "26" 
} 
*/
app.get("/boo" , (req , res)=>{
     res.send(req.body.name);
    console.log(req.body.name);
} )

//use query paramater 
// http://localhost:3000/qu?age=50
app.get("/qu" , (req , res ) =>{
    res.send(req.query);
} )


//article endpoint 
app.post ("/article" , async (req , res) => { 
    const newArt = new Article(); 

    const artT = req.body.artT ; 
    const artB = req.body.artB ; 
    newArt.title = artT ; 
    newArt.body = artB ; 
    newArt.nolikes = 0 ; 

    await newArt.save(); 
    
    res.json(newArt);
    
    res.send("article creation");
})


const PORT = 3000;

app.listen(PORT , ()=>{
    console.log(`Server is running on port ${PORT}`)
});