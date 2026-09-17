const calculatorContainer = document.getElementById("calculator-container"); 
const displayArea = document.getElementById("display-area");


let isError = false ; 
let isResult = false ; 

calculatorContainer.addEventListener('click' , e => {
    if(e.target.nodeName!="BUTTON") return ; 

    if(isError || isResult ){
        displayArea.textContent="";
        isError = false ; 
        isResult = false ;
    }

    switch(e.target.textContent){
        case 'c' :
            clear();
            break; 
        case 'DEL' :
            del();
            break;

        case '=':
            evaluate(); 
            break;

        default : 
            addToDisplay(e.target.textContent);
            break; 
    }
    
})

function clear(){
    displayArea.textContent="";
}

function del(){
    displayArea.textContent = displayArea.textContent.substring(0 , displayArea.textContent.length-1);
    if (displayArea.textContent=="invalid operation") displayArea.textContent="";
}

function evaluate(){
    try {
        let result = math.evaluate(displayArea.textContent);
        displayArea.textContent = result ;
        isResult = true; 
    }catch (error ){
        displayArea.textContent = "invalid operation"; 
        isError = true ; 
        console.error(error);
    }
    
}

function addToDisplay(val){
    displayArea.textContent += val;
}

