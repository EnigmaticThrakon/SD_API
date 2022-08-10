# SD_API

This repository will hold the project used to allow the device to be discovered by the intermediate server running this and allow the user
    that is operating the App to communicate with a connected device
    
To be run on the intermediate server that the device will connect to and the user's app will perform calls to

***Note:***

**This project is primarily configured to run in a Linux environment and it is unknown if it will successfully run on Windows or MacOS**

*Currently the development environment is a remote computer running `Ubuntu 20.04.4 LTS Desktop`*

***

**The following are the terminal commands are used for environment setup**

    sudo apt update 
    sudo apt upgrade -y 
    sudo apt install redis -y
    sudo apt-get install -y apt-transport-https
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb 
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    sudo apt-get update
    sudo apt-get install -y dotnet-sdk-6.0
    sudo apt install nginx -y
    sudo apt update
    sudo apt upgrade -y
    suto apt install mysql-server
    sudo mysql_secure_installation

***

**Nginx Requirements:**

*Once nginx is installed, the default sites-enabled file needs to be updated to the following (found in `/etc/nginx/enabled-sites/default`):*

    server {
        listen 80;

        server_name _;

        location / {
                proxy_pass http://127.0.0.1:5000;
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_cache_bypass $http_upgrade;
                proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header X-Forwarded-Proto $scheme;
        }
    }

*This will allow the server API to be accessed from outside computers using port `5000`*

***Note:*** 

**At this point the server can be reached from other devices that are within the same network, if it is to be accessed from outside the network then a dynamic DNS service should be used to gain access to the server's public IP and router port forwarding used to expose the necessary port to the outside**

***

**MySQL Requirements:**

*After MySQL is installed and the root user password is set, the following is done to prepare the database for the API application to connect and access the database:*

    CREATE DATABASE AgerDevice;
    CREATE USER 'user'@'localhost' IDENTIFIED BY 'password';
    GRANT ALL PRIVILEGES ON AgerDevice.* TO 'user'@'localhost' WITH GRANT OPTION;

*Changes to the database are handled through the Fluent Migrator tools that are setup in `AgerDevice > DataAccess > Migrations` and are automatically applied when the project is started*
