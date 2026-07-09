import { api } from '../api.js';
import { parseText } from '../Interactions/HashTags.js';


function getPostId(){  //get post id from url 
    const params = new URLSearchParams(window.location.search);
    return params.get("id");
}


export async function fetchComments(postId) {
  try{  
    
    const comments =   await api.get(`Comment/${postId}/comments`);
    renderComments(comments);
}catch (error) {
    console.error("Error fetching comments:", error);
}
}

function renderComments(comments) {
    const commentsContainer = document.getElementById("commentsContainer");
    commentsContainer.innerHTML = ""; // Clear existing comments
    const topLevel=comments.filter(c=>c.selfCommentId===null);
     
    function getReplies(commentId) {
        return comments.filter(c => c.selfCommentId === commentId)
    }

    topLevel.forEach(comment => {
        const card = createCommentCard(comment, getReplies);
        commentsContainer.appendChild(card);
    });

    if(comments.length === 0) {
        commentsContainer.innerHTML = 
    `<p class='text-secondary'>No comments yet. Be the first to comment!</p>`;
}
}

function createCommentCard(comment, getReplies) {
    const div = document.createElement("div");
    div.className = "comment-card mb-2";
    div.innerHTML = `
        <div class="d-flex gap-2">
            <div class="flex-grow-1">
                <span class="fw-semibold">${comment.authorUsername}</span>
            <p class="mb-1 comment-body" id="comment-body-${comment.id}">
             ${parseText(comment.body)}
              </p>

                 

                <div class="d-flex gap-1 mt-2">
                    <div class="reaction-total-box " id="reaction-count-${comment.id}">
                 ${comment.likeCount || 0}
                  </div>
                    <button class="btn-purple comment-reaction"
                        data-reacted="false"
                        onclick="toggleCommentReaction(this, ${getPostId()}, ${comment.id}, 'Like')">
                        👍 
                    </button>

                    <button class="btn-purple comment-reaction"
                        data-reacted="false"
                        onclick="toggleCommentReaction(this, ${getPostId()}, ${comment.id}, 'Haha')">
                        😂
                    </button>

                    <button class="btn-purple comment-reaction"
                        data-reacted="false"
                        onclick="toggleCommentReaction(this, ${getPostId()}, ${comment.id}, 'love')">
                        ❤️
                    </button>
                    ${!comment.isOwner ? `
                     <a href="../../html/Report/Report-modal.html?commentId=${comment.id}" class="btn-purple">
                       <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-postage-fill" viewBox="0 0 16 16">
  <path d="M4.5 3a.5.5 0 0 0-.5.5v9a.5.5 0 0 0 .5.5h7a.5.5 0 0 0 .5-.5v-9a.5.5 0 0 0-.5-.5z"/>
  <path d="M3.5 1a1 1 0 0 0 1-1h1a1 1 0 0 0 2 0h1a1 1 0 0 0 2 0h1a1 1 0 1 0 2 0H15v1a1 1 0 1 0 0 2v1a1 1 0 1 0 0 2v1a1 1 0 1 0 0 2v1a1 1 0 1 0 0 2v1a1 1 0 1 0 0 2v1h-1.5a1 1 0 1 0-2 0h-1a1 1 0 1 0-2 0h-1a1 1 0 1 0-2 0h-1a1 1 0 1 0-2 0H1v-1a1 1 0 1 0 0-2v-1a1 1 0 1 0 0-2V9a1 1 0 1 0 0-2V6a1 1 0 0 0 0-2V3a1 1 0 0 0 0-2V0h1.5a1 1 0 0 0 1 1M3 3v10a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V3a1 1 0 0 0-1-1H4a1 1 0 0 0-1 1"/>
</svg> Report Comment
                    </a>
                    ` : ""}
                   
                </div>

   <div class="d-flex gap-3 mt-1">
    <small class="text-secondary">${formatTime(comment.dateCreated)}</small>

    <button class="reply-btn btn btn-link btn-sm p-0">
        Reply
    </button>

${comment.isOwner && canEditComment(comment.dateCreated) ? `
    <button class="edit-comment-btn btn btn-link btn-sm p-0">
        Edit
    </button>

    <button class="delete-comment-btn btn btn-link btn-sm p-0 text-danger">
        Delete
    </button>
` : `
    ${comment.isOwner ? `
        <button class="delete-comment-btn btn btn-link btn-sm p-0 text-danger">
            Delete
        </button>
    ` : ""}
`}

    <button class="toggle-replies btn btn-link btn-sm p-0 text-secondary">
        ▶ Replies
    </button>
</div>
            </div>
        </div>

        <div class="replies ms-4 mt-2 d-none" id="replies-${comment.id}"></div>
    `;

    // Replies
    const repliesContainer = div.querySelector(`#replies-${comment.id}`);
    const replies = getReplies(comment.id);

    replies.forEach(reply => {
        repliesContainer.appendChild(createCommentCard(reply, getReplies));
    });

    // Reply button
    div.querySelector(".reply-btn")
        ?.addEventListener("click", () => showReplyForm(comment.id));

    // Delete button
    div.querySelector(".delete-comment-btn")
        ?.addEventListener("click", () => deleteComment(comment.id));
    //edit button
        div.querySelector(".edit-comment-btn")
    ?.addEventListener("click", () => editComment(comment));

    // Toggle replies
    const toggleBtn = div.querySelector(".toggle-replies");

    if (replies.length === 0) {
        toggleBtn.style.display = "none";
    } else {
        toggleBtn.addEventListener("click", () => {
            repliesContainer.classList.toggle("d-none");

            toggleBtn.textContent =
                repliesContainer.classList.contains("d-none")
                    ? "▶ Replies"
                    : "▼ Replies";
        });
    }

    return div;
}

export async function addComment(postId,body,selfCommentId=null){
try{
   
  await api.post(`Comment/${postId}/comments`,{
    body: body,
    selfCommentId: selfCommentId
});
 await fetchComments(postId);
}catch(error){
    console.error("Error adding comment:", error)
}
}
export async function deleteComment(commentId) {
    console.log("Deleting comment:", commentId);
    try {
        const result = await api.delete(`Comment/${commentId}/deleteComment`);
        console.log("Delete result:", result);
        const postId = getPostId();
        await fetchComments(postId);
    } catch(error) {
        console.error("Error deleting comment:", error);
    }
}
function formatTime(dateString) {
    const date = new Date(dateString+"Z");
    const now = new Date();
    const diffInMins = Math.floor((now - date) / 60000);

    if (diffInMins < 1)    return "Just now";
    if (diffInMins < 60)   return `${diffInMins} minutes ago`;
    if (diffInMins < 1440) return `${Math.floor(diffInMins / 60)} hours ago`;
    
    return `${Math.floor(diffInMins / 1440)} days ago`;
   
    
}
function showReplyForm(selfCommentId) {
    const postId = getPostId();

    const existing = document.getElementById(`reply-form-${selfCommentId}`);
    if (existing) {
        existing.remove();
        return;
    }

    const repliesContainer = document.getElementById(`replies-${selfCommentId}`);
    if (!repliesContainer) {
        console.log('replies container not found!', selfCommentId);
        return;
    }
    repliesContainer.classList.remove('d-none');
    
    const form = document.createElement("div");
    form.id = `reply-form-${selfCommentId}`;
    form.className = "ms-4 mt-2";
    form.innerHTML = `
        <div class="input-group">
            <input type="text" 
                   class="form-control form-control-sm" 
                   id="reply-input-${selfCommentId}"
                   placeholder="Write a reply...">
            <button class="btn btn-sm btn-purple" 
                    onclick="submitReply(${selfCommentId})">
                Reply
            </button>
        </div>
    `;

   
    repliesContainer.prepend(form);
    
}

async function submitReply(selfCommentId) {
    const input = document.getElementById(`reply-input-${selfCommentId}`);
    if(!input) return;

    const body = input.value.trim();
    if (!body) return;

    const postId = getPostId();
    await addComment(postId, body, selfCommentId);
    const repliesContainer = document.getElementById(`replies-${selfCommentId}`);
    if (repliesContainer) {
        repliesContainer.classList.remove('d-none');
    }
}
function editComment(comment) {

    const body = document.getElementById(`comment-body-${comment.id}`);

    body.innerHTML = `
        <textarea
            id="edit-comment-${comment.id}"
            class="form-control mb-2"
            rows="3">${comment.body}</textarea>

        <button class="btn btn-purple btn-sm"
            onclick="saveComment(${comment.id})">
            Save
        </button>

        <button class="btn btn-secondary btn-sm ms-2"
            onclick="cancelEdit(${comment.id}, \`${comment.body.replace(/`/g,"\\`")}\`)">
            Cancel
        </button>
    `;
}
function cancelEdit(commentId, originalBody) {

  const body = document.getElementById(`comment-body-${commentId}`);

    body.innerHTML = parseText(originalBody);
}
async function saveComment(commentId) {

    const newBody =
        document.getElementById(`edit-comment-${commentId}`).value.trim();

    if (!newBody) return;

    try {

        await api.put(`Comment/${commentId}/editComment`, {
            body: newBody
        });

        await fetchComments(getPostId());

    } catch (err) {
        console.error(err);
    }
}
function canEditComment(dateString) {
    const created = new Date(dateString + "Z");
    const now = new Date();

    const diffInMinutes = (now - created) / 60000;

    return diffInMinutes <= 5;
}

window.saveComment = saveComment;

window.cancelEdit = cancelEdit;

window.submitReply = submitReply; 
window.showReplyForm = showReplyForm;