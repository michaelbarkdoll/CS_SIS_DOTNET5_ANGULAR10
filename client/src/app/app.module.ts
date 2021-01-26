import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { SharedModule } from 'src/app/_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { ConfirmDialogComponent } from './modals/confirm-dialog/confirm-dialog.component';
import { WebinfoComponent } from './webinfo/webinfo.component';
import { WeburlComponent } from './weburl/weburl.component';
import { DockerComponent } from './docker/docker.component';
import { GuacamoleLinuxComponent } from './guacamole-linux/guacamole-linux.component';
import { GuacamoleWindowsComponent } from './guacamole-windows/guacamole-windows.component';
import { VdiComponent } from './vdi/vdi.component';
import { VpnComponent } from './vpn/vpn.component';
import { ResearchlabComponent } from './researchlab/researchlab.component';
import { PrintjobsComponent } from './printjobs/printjobs.component';
import { PrintquotaComponent } from './printquota/printquota.component';
import { PrintsubmitComponent } from './printsubmit/printsubmit.component';
import { CwpsComponent } from './cwps/cwps.component';
import { MysqlComponent } from './mysql/mysql.component';
import { OracleComponent } from './oracle/oracle.component';
import { NosqlComponent } from './nosql/nosql.component';
import { SshComponent } from './ssh/ssh.component';
import { MicrosoftComponent } from './microsoft/microsoft.component';
import { HelpComponent } from './help/help.component';
import { PrinterAdminViewComponent } from './printer/printer-admin-view/printer-admin-view.component';
import { PrinterAdminManagementComponent } from './printer/printer-admin-management/printer-admin-management.component';
import { PrinterJobManagementComponent } from './printer/printer-job-management/printer-job-management.component';
import { PrinterAdminStatusDialogComponent } from './printer/printer-admin-status-dialog/printer-admin-status-dialog.component';
import { PrintJobAdminStatusDialogComponent } from './printer/print-job-admin-status-dialog/print-job-admin-status-dialog.component';
import { WebadminComponent } from './webadmin/webadmin.component';
import { UserAccessManagementComponent } from './admin/user-access-management/user-access-management.component';
import { SemesterManagementComponent } from './admin/semester-management/semester-management.component';
import { UploadClasslistComponent } from './admin/upload-classlist/upload-classlist.component';
import { ProcessClasslistComponent } from './admin/process-classlist/process-classlist.component';
import { DockerContainerViewComponent } from './docker/docker-container-view/docker-container-view.component';
import { DockerRequestContainerComponent } from './docker/docker-request-container/docker-request-container.component';
import { DockerAdminViewComponent } from './docker/docker-admin-view/docker-admin-view.component';
import { DockerContainerStatusDialogComponent } from './docker/docker-container-status-dialog/docker-container-status-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputComponent,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
    ConfirmDialogComponent,
    WebinfoComponent,
    WeburlComponent,
    DockerComponent,
    GuacamoleLinuxComponent,
    GuacamoleWindowsComponent,
    VdiComponent,
    VpnComponent,
    ResearchlabComponent,
    PrintjobsComponent,
    PrintquotaComponent,
    PrintsubmitComponent,
    CwpsComponent,
    MysqlComponent,
    OracleComponent,
    NosqlComponent,
    SshComponent,
    MicrosoftComponent,
    HelpComponent,
    PrinterAdminViewComponent,
    PrinterAdminManagementComponent,
    PrinterJobManagementComponent,
    PrinterAdminStatusDialogComponent,
    PrintJobAdminStatusDialogComponent,
    WebadminComponent,
    UserAccessManagementComponent,
    SemesterManagementComponent,
    UploadClasslistComponent,
    ProcessClasslistComponent,
    DockerContainerViewComponent,
    DockerRequestContainerComponent,
    DockerAdminViewComponent,
    DockerContainerStatusDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule, // SharedModules that we add to angular (ToastrModule [ngx-toastr], BsDropdownModule [ngx-bootsrap/dropdown])
    NgxSpinnerModule  
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
