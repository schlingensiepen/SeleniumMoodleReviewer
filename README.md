# SeleniumMoodleReviewer
Mock Up - How a fast review gui for Moodle should look like ...


Howto use:
- Setup
  + Clone Repo
  + Replace 'WindowsFormsApplication1\chromedriver.exe' with a suitable version
- Start
  + Open Moodle Grading Page and copy its URL
  + Download all Solutions in one zip-Archive from moodle
  + Unzip this Archive to C:\moodle
- Run Application
  + Insert URL
  + Click 'Open'
  + An new Chrome Window opens moodle
    - Login
    - Set View to 'need review'
  + App prompts "Unpack your archive to ..."
  + Press OK
- Do the Review 
  + Progress
    - Press Next to go to next Submission (or start with first one)
    - Press Store to fill your current grade to the moodle form and go to next submission  
  + Grading
    - Press '0' to set grade empty --> not approved
    - Press '100' to set grade to 100 --> not approved
    - Press open to open the folder
    - Insert a comment in the textfield
    - OR: Select a comment from the list to send it to text field
    - Do not forget to press 'Store'
  + Submit Grades
    - All data is not set to moodle but filled in the html form so you need to submit the form in chrome
    
  
