import { api } from '../api.js';

window.onload= () =>{
    var myModal = new bootstrap.Modal(document.getElementById('reportModal'));
    myModal.show();
    const params = new URLSearchParams(window.location.search);
    const commentId = params.get("commentId");
    const postId = params.get("postId");
    var submitbtn = document.getElementById("submitReportBtn");
    var cancelbtn = document.getElementById("cancel-report-btn");
    const descriptionInput = document.getElementById("reportDescription");

    cancelbtn.addEventListener('click',()=>{
        window.location.href = `../../html/Posts/Feed.html`;
    });

    submitbtn.addEventListener('click', async (e) => {
        e.preventDefault();

        const selectedReasonEl = document.querySelector(
            'input[name="reportReason"]:checked'
        );

        if (!selectedReasonEl) {
           errorMsg.textContent = 'Please select a reason!';
            errorMsg.style.display = 'block';
            return;
        }

        const reportData = {
            postId: postId ? parseInt(postId) : null,
            commentId: commentId ? parseInt(commentId) : null,
            reason: selectedReasonEl.value,
            description: descriptionInput.value
        };

    try{
        const result = await api.post("Report/submitreport", reportData);
        successMsg.textContent = 'Report submitted successfully';
        successMsg.style.display = 'block';
        window.location.href = `../../html/Posts/Feed.html`;
    }
    catch(error){
          errorMsg.textContent = "Couldn't submit report" + error.message;
            errorMsg.style.display = 'block';
    }
    });

}

function goBack() {
    window.history.back();
}
window.goBack = goBack;