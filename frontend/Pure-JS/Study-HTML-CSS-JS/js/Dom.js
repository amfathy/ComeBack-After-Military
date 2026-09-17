/*

get attribute 
set attribute 
remove attribute 
hasAttribute 

*/

const myLink = document.querySelector(".main-link");
console.log(myLink);

//* getAttriburte  

console.log( myLink.getAttribute("class"));
console.log( myLink.getAttribute("src"));

//* set attribute 
// used to add an attribute 

myLink.setAttribute("target" , "_blank");
myLink.setAttribute("title" , "fathy's site");

myLink.removeAttribute("title");

console.log(myLink.hasAttribute("target"));

/*innerHTML  , innerText , textContent */
const firstpara = document.querySelector("p");
console.log(firstpara);

//innerHTML
console.log(firstpara.innerHTML);
firstpara.innerHTML="<h1> heading <h1>"
//---------------------------------------------
//innertext 
//return every thing inside tag but not take tags
//remove spaces 
// remove style 
// firstpara.innerText = "text inner";

//-----------------------------------------------
//text content 
// respect spaces and style 
firstpara.textContent = "textContent";
