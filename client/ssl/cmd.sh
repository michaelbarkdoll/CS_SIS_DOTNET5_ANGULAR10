openssl req -newkey rsa:2048 -nodes -keyout privkey.pem -x509 -days 36500 -out certificate.pem -subj "/C=US/ST=NRW/L=Earth/O=CompanyName/OU=IT/CN=www.example.com/emailAddress=email@example.com"

export NODE_OPTIONS="max_old_space_size=8096"
ng serve --ssl --ssl-cert certificate.pem --ssl-key privkey.pem

