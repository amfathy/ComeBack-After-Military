import mongoose from "mongoose";
const schema = mongoose.Schema ; 


const articleschema = new schema({
title : String , 
body : String ,
nolikes : Number 
});

const Article = mongoose.model("Article" , articleschema);

export default Article;