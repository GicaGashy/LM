  function now(){
    var d = new Date();
    var output = d.getDate() + "-" + (1 + d.getMonth()) + "-" + d.getFullYear();
    console.log(output);
    document.getElementById('g-eod-form').value = output;
    return output;
  }
