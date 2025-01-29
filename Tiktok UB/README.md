# **TikTok Automation Tool: Setup Guide**

## **Follow These Steps Carefully**

### **Step 1: Install Google Chrome**
- Download and install the latest version of Google Chrome: [Download Chrome](https://www.google.com/chrome/).

### **Step 2: Locate Chrome's File Path**
1. Press `Win + S` or use the search button.
2. Search for **"Google Chrome"** and select **"Open File Location"**.
3. Right-click **Google Chrome** and choose **Properties**.
4. Find the **"Target"** field and copy the file path (e.g., `"C:\Program Files\Google\Chrome\Application\chrome.exe"`).

### **Step 3: Enable Debugging in Chrome**
1. Open the **Command Prompt** (press `Win + S` or search for "Command Prompt").
2. Paste the following command:
   ```cmd
   "C:\Program Files\Google\Chrome\Application\chrome.exe" --remote-debugging-port=9222 --user-data-dir="C:\ChromeDebug"
   ```
3. Press Enter. A debugging-enabled Chrome browser will open.
4. Log in to TikTok in this browser.

### **Step 4: Run the Automation Tool**
1. Open the automation tool (e.g., `Tiktok UB.exe`).
2. Enter your **TikTok username** (not your nickname) into the username field.
3. Click **Start**.

---

## **Adding Exclusions**
To exclude specific users from being unfollowed:
1. Click the **"+" button** in the application.
2. Copy the **nickname** of the user you want to exclude.
3. Paste the nickname into the text field and click **Submit**.  
   *You can add multiple exclusions by repeating this process.*

### **Important Notes**
- Ensure you also paste your **TikTok username** (not the nickname) into the main username field.  
- Do **not** include the `@` symbol, as it is automatically included.

### **Start the Tool**
- Once exclusions are set and your username is entered, click **Start** to begin unfollowing users (excluding those added).

---

## **Disclaimer**
If Windows Defender flags or deletes any files:
- Add an exclusion in your **Virus & Threat Protection settings**.

Since this tool is open-source, feel free to review the code for peace of mind!
