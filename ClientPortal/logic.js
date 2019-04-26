var sendImageForAnalysis = function(files){
  var formData = new FormData();
  formData.append('image', files); 

  fetch("http://localhost:7071/api/UploadImage?UserName=Crash Override",
  {
    "Content-Type": "multipart/form-data",
      body: formData,
      method: "post"
  }).then(function(res){
    return res.json();
  }).then(function(data){
    var res = data.content;
    checkAnalysisResults(res.sessionId, res.commandId, 'Crash Override', res.awaitTimePeriodMiliseconds)
  });
};

var checkAnalysisResults = function(sessionId, commandId, userName, delay){
    
    setTimeout(function(){ 
      var url = 'http://localhost:7071/api/CheckCommandResult?sessionId=' + sessionId + '&commandId=' + commandId + '&userName=' + userName;

      fetch(url).then(function(res){
        return res.json();
      }).then(function(data){
        var shouldWaitLonger = data.awaitTimePeriodMiliseconds != null;
        if(shouldWaitLonger)
          checkAnalysisResults(data.sessionId, data.commandId, 'Crash Override', data.awaitTimePeriodMiliseconds)
        else
          displayAnalysisResult(data.celebrities);
      });
     }, delay);

}

var displayAnalysisResult = function(arrayOfCelebrities){
  var celebrities = arrayOfCelebrities.join(', ');
  $('#analysis-result').text(celebrities);
}

$( document ).ready(function() {

  document.getElementById("detect-button").onclick = function() {
    document.getElementById("file-browser").click();  
  };
  
  document.getElementById("file-browser").onchange = function () {
    if (this.files && this.files[0]) {
      var reader = new FileReader();

      reader.onload = function (e) {
          $('#preview-image')
              .attr('src', e.target.result)
              //.width(150)
              .height('50%');
      };
      
      reader.readAsDataURL(this.files[0]);
      sendImageForAnalysis(this.files[0]);
  }
  
  }; 

});
