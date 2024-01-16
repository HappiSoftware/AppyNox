# Adding New Service

In another project, you can follow these steps when creating a new service:

1. **First, open a folder with the new service name in the 'src/Services' directory. Then create Domain, Application, Infrastructure, SharedEvents and WebAPI projects. For example 'Service Name: Constant'. Check out the image below.**

<details>
<summary>Click to expand the image</summary>

![addingnewservice](_media/development/addingnewservice1.png)

</details>

<br>

2. **Install necessary Nox packages for each layer.**

<details>
<summary>Click to expand the image</summary>

![installingnugets](_media/development/installingnugetpackages.png)

</details>

<br>

3. **Create your entities in Domain layer.**

<details>
<summary>Click to expand the image</summary>

![entities](_media/development/entities.png)

</details>

<br>

4. **Create your Dtos and Dto mappings in Application layer.**

<details>
<summary>Click to expand the image</summary>

![dtos](_media/development/creatingdtos.png)

</details>

<br>

5. **Create other tools and make same structure like below.**

<details>
<summary>Click to expand the image</summary>

![applicationlayer](_media/development/applicationlayer.png)

</details>

<br>

6. **Create entity configurations and then a DbContext in Infrastructure layer.**

<details>
<summary>Click to expand the image</summary>

![dbconf](_media/development/dbconf.png)

</details>

<br>

7. **Create other tools and make same structure like below.**

<details>
<summary>Click to expand the image</summary>

![infralayer](_media/development/infralayer.png)

</details>

<br>

8. **Create SharedEvents class library project to use for common events between microservices.**

<details>
<summary>Click to expand the image</summary>

![shevents](_media/development/shevents.png)

</details>

<br>

9. **Create WebAPI project and make same structure like below.**

<details>
<summary>Click to expand the image</summary>

![webapilayer](_media/development/webapilayer.png)

</details>

<br>