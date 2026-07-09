import { api } from '../api.js';
async function toggleReaction(button, postId, reactionType) {
    const container = button.parentElement;
    const currentReaction = container.querySelector(".reacted");
    
   
    const countBox = document.getElementById(`reaction-count-${postId}`);
    const currentCount = parseInt(countBox?.textContent) || 0;

    if (currentReaction === button) {
        const success = await removeReaction(postId);
      if (success) {
    button.classList.remove("reacted");
    button.dataset.liked = "false";

    const post = await api.get(`Post/${postId}`);

    if (countBox) {
        countBox.textContent = post.likeCount;
    }
}
        return;
    }

    if (currentReaction) {
        await removeReaction(postId);
        currentReaction.classList.remove("reacted");
        currentReaction.dataset.liked = "false";
        
    }

const success = await addReaction(postId, reactionType);

if (success) {
    button.classList.add("reacted");
    button.dataset.liked = "true";


    const post = await api.get(`Post/${postId}`);

    if (countBox) {
        countBox.textContent = post.likeCount;
    }
}
    
}

let reactionCallCount = 0;
async function addReaction(postId, reactionType) {
 console.log(`addReaction call #${reactionCallCount}`, new Date().getTime());
    console.trace();

    try {
        const data = await api.post("Reaction/addPostreaction", {
            postId: Number(postId),
            reactionType: reactionType
        });

        console.log("Reaction added:", data);
        return true;
    } catch (error) {
        console.error("Error adding reaction:", error);
        return false;
    }
}


async function removeReaction(postId) {
    try{
     await api.delete(`Reaction/removePostreaction?postId=${postId}`);
        return true;
     } catch (error) {
        console.error("Error removing reaction:", error);
        return false;
    }
}

async function addCommentReaction(postId, commentId, reactionType) {
try{
        const data = await api.post("Reaction/addCommentreaction", {
            postId: Number(postId),
            commentId: Number(commentId),
            reactionType: reactionType
        });
       
        console.log("Comment reaction added:", data);
        return true;
    } catch (error) {
        console.error("Error adding comment reaction:", error);
        return false;
    }
}

async function removeCommentReaction(postId, commentId) {
    try {
        await api.delete(`Reaction/removeCommentreaction?postId=${postId}&commentId=${commentId}`);
        return true;
    } catch (error) {
        console.error("Error removing comment reaction:", error);
        return false;
    }
}


async function toggleCommentReaction(button, postId, commentId, reactionType) {
    const container = button.parentElement;
    const currentReaction = container.querySelector(".reacted");

    
    const countBox = document.getElementById(`reaction-count-${commentId}`);
    const currentCount = parseInt(countBox?.textContent) || 0;

    if (currentReaction === button) {
        const success = await removeCommentReaction(postId, commentId);
        if (success) {
            button.classList.remove("reacted");
            button.dataset.reacted = "false";
            if (countBox) countBox.textContent = Math.max(0, currentCount - 1);
        }
        return;
    }

    if (currentReaction) {
        await removeCommentReaction(postId, commentId);
        currentReaction.classList.remove("reacted");
        currentReaction.dataset.reacted = "false";
    }

    const success = await addCommentReaction(postId, commentId, reactionType);
    if (success) {
        button.classList.add("reacted");
        button.dataset.reacted = "true";
        if (!currentReaction && countBox) {
            countBox.textContent = currentCount + 1;
        }
    }
}


window.addReaction = addReaction;
window.removeReaction=removeReaction;
window.toggleReaction = toggleReaction;
window.toggleCommentReaction = toggleCommentReaction;