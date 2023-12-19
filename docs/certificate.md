# SSL Certificates

After cloning the repository, add development SSL certificates to the services for running them in a Docker container. Run the following commands to generate and trust the SSL certificates:

1. **Download OpenSSL**

   Navigate to https://slproweb.com/products/Win32OpenSSL.html and download OpenSSL installer.

   `(Win64 OpenSSL v3.2.0 Light)` version may differ.

<br>

2. **Edit Environment Variables**

   Add `C:\Program Files\OpenSSL-Win64\bin` to System variables. Path may change based on your installation location.

<br>

3. **Create `cert.config`**

   In a folder create `cert.config`.

<details>
    <summary>The content of the cert.config Example</summary>

```config
[req]
default_bits = 2048
prompt = no
default_md = sha256
x509_extensions = v3_req
distinguished_name = dn

[dn]
C=TR
ST=Sakarya
L=Sakarya
O=HappiSoftware
OU=NeonNinjas
emailAddress=happisoftware@gmail.com
CN = localhost

[v3_req]
subjectAltName = @alt_names

[alt_names]
DNS.1 = appynox.gateway.ocelotgateway
DNS.2 = appynox.consul
DNS.3 = appynox.services.authentication.webapi
DNS.4 = appynox.services.coupon.webapi
```

</details>

<br>

4. **Create pfx and crt files**

Run the following codes in the terminal.

```bash
openssl req -new -nodes -newkey rsa:2048 -keyout gateway-service.key -out gateway-service.csr -config cert.config
openssl x509 -signkey gateway-service.key -in gateway-service.csr -req -days 365 -out gateway-service.crt -extfile cert.config -extensions v3_req
openssl pkcs12 -export -out gateway-service.pfx -inkey gateway-service.key -in gateway-service.crt -password pass:happi2023
```

<br>

5. **Adding the files to APIs**

   In each api project there will be `'ssl'` and `'ssl/crt'` folders. Move the `.pfx` file under `'ssl'` and `.crt` file under `'ssl/crt'` folder. Also **don't forget** to rename the files. Each pfx and crt file should be named `'example-service.pfx'` or `'example-service.crt'`. If you are not sure about the namings you can navigate to `docker.compose` and find the related service. You can copy the names of the files from there.

<br>

6. **Trusting the crt in Docker Important**

   If you are getting 502 from api calls such as through gateway, it is most likely because your docker container is not trusting. To trust the crt find your container first with and copy the id of the container:

   ```bash
   docker ps
   ```

   After that execute:

   ```bash
   docker exec -u root -it {your-docker-container} bash
   update-ca-certificates
   ```
