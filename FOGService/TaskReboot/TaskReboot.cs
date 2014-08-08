﻿
using System;

namespace FOG {
	/// <summary>
	/// Reboot the computer if a task needs to
	/// </summary>
	public class TaskReboot : AbstractModule {
		
		private Boolean notifiedUser; //This variable is used to detect if the user has been told their is a pending shutdown
		
		public TaskReboot():base(){
			setName("TaskReboot");
			setDescription("Reboot if a task is scheduled");
			this.notifiedUser = false;
		}
		
		protected override void doWork() {
			//Get task info
			Response taskResponse = CommunicationHandler.getResponse("/fog/service/jobs.php?mac=" + CommunicationHandler.getMacAddresses());
				
			//Shutdown if a task is avaible and the user is logged out or it is forced
			if(!taskResponse.wasError() && (!UserHandler.isUserLoggedIn() || taskResponse.getField("#force").Equals("1") )) {
				ShutdownHandler.restart(getName(), 30);
			} else {
				if(!this.notifiedUser) {
					NotificationHandler.createNotification(new Notification("Please log off", NotificationHandler.getCompanyName() + 
					                                                        " is attemping to service your computer, please log off at the soonest available time", 
					                                                        60));
					this.notifiedUser = true;
				}
			}
			
		}
		
	}
}