//==============================================================
/*Create a function that returns a new array containing
only the even numbers.*/

 function getEven(arr) {
    let result=[]; 
    for (const ele of arr){
        if (ele %2 == 0 ) 
            result.push(ele);
    }
    return result ;
}

//another solution

const arr = [3,5,4,1,8,8];
const res = arr.filter((num)=>num%2==0)

//==============================================================

/*Create a function that receives an array of numbers
and returns:

{
    positive: ?,
    negative: ?,
    zero: ?
}

Example:

[-5, 10, 0, 20, -3, 0]

Expected:

{
    positive: 2,
    negative: 2,
    zero: 2
}
*/

function detectNumbers(arr){
    const num = {
        pos : 0 ,
        neg : 0 , 
        zero : 0 
    }
    for(let ele of arr){
        if(ele < 0)
            num.neg++;
        else if (ele>0)
            num.pos++;
        else num.zero++;
    }
    return num;
} 
//==============================================================

/*3. REMOVE DUPLICATES
------------------------------------------------------------
Create a function that receives:

[1, 2, 2, 3, 4, 4, 5, 5]

and returns:

[1, 2, 3, 4, 5]
*/

function removeDuplicates (arr){
    let res = new Set();
    for(let ele of arr){
        res.add(ele);
    }
    return res ;
}

//==============================================================
/*SUM OF ARRAY
------------------------------------------------------------
Create:

calculateSum(numbers)

Use reduce() to calculate the total.

Example:

[10, 20, 30, 40]

Expected:

100
*/
var numbers = [3,4,5,8,1,4,44];
const sum = numbers.reduce((acc , curr)=>{
    return acc + curr;
},0);

//==============================================================
/*5. ARRAY TRANSFORMATION
------------------------------------------------------------
Given:

const numbers = [2, 4, 6, 8];

Create a new array where every number is multiplied
by 3.
*/
var numbers = [2, 4, 6, 8];

const res5 = numbers.map((num) => num*3);

//==============================================================

/*
9. FILTER USERS
------------------------------------------------------------
Given:

const users = [
    { name: "Ahmed", age: 25 },
    { name: "Ali", age: 17 },
    { name: "Sara", age: 30 },
    { name: "Omar", age: 15 }
];

Return users who are 18 or older.
*/

const users = [
    { name: "Ahmed", age: 25 },
    { name: "Ali", age: 17 },
    { name: "Sara", age: 30 },
    { name: "Omar", age: 15 }
];

const res6 = users.filter((user)=>user.age>18);
console.log(res6);

//==============================================================
/*
Using the same users array:

Create:

findUser(id)

Return the user with the given ID.

If the user doesn't exist, return:

null

Use find().
*/

const users6 = [
    { name: "Ahmed", age: 25 , id : 2022 },
    { name: "Ali", age: 17 , id : 2025},
    { name: "Sara", age: 30 , id : 2024},
    { name: "Omar", age: 15 , id : 2020}
];

const res7 = users6.find((us) => us.id==2026 );
res7 == undefined ? console.log("NULL") : console.log(res7) ;  

//==============================================================
/*12. FREQUENCY COUNTER
------------------------------------------------------------
Given:

const numbers = [1, 2, 2, 3, 3, 3, 4];

Return:

{
    1: 1,
    2: 2,
    3: 3,
    4: 1
}

Use loops and objects.
*/
const numbers8 = [1, 2, 2, 3, 3, 3, 4];
 const res8 = {}; 
 for(num of numbers8){
    if(!res8[num]) res8[num] = 1 ;
    else res8[num]++;
 }



//==============================================================
/*9. PALINDROME
------------------------------------------------------------
Create:

isPalindrome(word)

Examples:

isPalindrome("madam")
→ true

isPalindrome("hello")
→ false

Do not use a library.*/
const word = "ahmed" ; 
let size = word.length;
let i=0 , j=size-1;
let palindrome = true;  
while (i<size/2 , j>size/2 ){
    if (word[i] != word[j] )
    {
        palindrome=false; 
        break;
    }
    i++ ; j--;
}
console.log(palindrome);


//==============================================================
/*Given:

const words = [  "javascript",  "node", "backend",  "api"];
Return:
[ 10, 4, 7,3]
Use map().
*/
const words = [  "javascript",  "node", "backend",  "api"];
const res10 = words.map((word)=>word.length);
console.log(res10);

//==============================================================
/**/


//==============================================================
/**/