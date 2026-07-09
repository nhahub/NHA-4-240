import { api } from '../api.js';


async function getNotifications() {
document.getElementById("loading").classList.remove("d-none");
try{
   
    const notifications = await api.get("Notification/getnotification")
    renderNotifications(notifications);
}
catch (error) {
    console.error("Error fetching notifications:", error);
}
finally {
    document.getElementById("loading").classList.add("d-none");
}
}


function renderNotifications(notifications) {
  

  const list=document.getElementById("notifications-list");
  list.innerHTML = "";

  if(notifications.length===0){
    document.getElementById("no-notifications").classList.remove("d-none");
    return;
  }
  document.getElementById("no-notifications").classList.add("d-none");
  notifications.forEach(notification=>{
    const card=createNotificationCard(notification);
    list.appendChild(card);
  });
}

function createNotificationCard(notification) {
    const card = document.createElement("div");
    card.className = `notification-card p-3 d-flex align-items-center gap-3
        ${!notification.isSeen ? "unread" : ""}`;
    card.innerHTML = `
        <div class="rounded-circle d-flex justify-content-center align-items-center flex-shrink-0"
             style="width:45px;height:45px;background:#2a2a4a;font-size:1.2rem">
            ${getIcon(notification.type)}
        </div>

        <div class="flex-grow-1">
   <div class="notification-title fw-semibold">
    ${notification.message}
</div>
           <small
            title="${new Date(notification.dateCreated).toLocaleString()}"
              class="text-secondary">
             ${formatTime(notification.dateCreated)}
            </small>
        </div>

        ${!notification.isSeen 
            ? '<span class="badge-new">New</span>' 
            : ''}
    `;

   card.onclick = () => openNotification(notification);
    return card;
}

async function openNotification(notification) {
    try {
        if (!notification.isSeen) {
            await api.patch(`Notification/readNotification?id=${notification.id}`);
            await loadUnreadCount();
        }

        switch (notification.type) {
            case "Comment":
            case "Reaction":
            case "Mention":
                window.location.href = `../Posts/PostDetail.html?id=${notification.referenceId}`;
                break;

            case "Follow":
                window.location.href = `../User/OtherUserProfile.html?userId=${notification.senderId}`;
                break;

            default:
                getNotifications();
               
        }

    } catch (error) {
        console.error(error);
    }
}
function getIcon(type) {
    switch(type){
        case "Reaction":
            return "❤️";

        case "Comment":
            return "💬";

        case "Follow":
            return "👤";

        case "Mention":
            return "@";

        case "Report":
            return "🚩";

        default:
            return "🔔";
    }
}
function formatTime(dateString) {
    const date = new Date(dateString + "Z");
    const now = new Date();
    const diffInMins= Math.floor((now - date) / 60000);
    if (diffInMins < 1) return "Just now";
    if (diffInMins < 60) return `${diffInMins} minutes ago`;
    if (diffInMins < 1440) return `${Math.floor(diffInMins / 60)} hours ago`;
    return `${Math.floor(diffInMins / 1440)} days ago`;
}

async function markAsRead(notificationId) {
    try {
        await api.patch(`Notification/readNotification?id=${notificationId}`);
        getNotifications();
       
    }
    catch (error) {
        console.error("Error marking notification as read:", error);
    }
}
document.addEventListener("DOMContentLoaded", () => {
    getNotifications();
   
    loadUnreadCount();
    document.getElementById("clear-notifications").addEventListener("click", markAllAsRead);
});

async function markAllAsRead() {
    try {
       await api.patch("Notification/readAllNotifications")
        getNotifications();

        loadUnreadCount()
    }
    catch (error) {
        console.error("Error marking all notifications as read:", error);
    }
}

async function loadUnreadCount() {
    try{
     
    const data = await api.get("Notification/GetUnreadCount");
    const badge=document.getElementById("notification-count");
    if (badge) {
    if (data.unreadCount > 0) {
        badge.textContent = data.unreadCount;
        badge.style.display = "inline-block";
    } else {
        badge.style.display = "none";
    }
}

}catch (error) {
    console.error("Error fetching unread count:", error);
}   
}

function goBack() {
    window.history.back();
}
window.goBack = goBack;