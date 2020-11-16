import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { CwpsComponent } from './cwps/cwps.component';
import { DockerComponent } from './docker/docker.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { GuacamoleLinuxComponent } from './guacamole-linux/guacamole-linux.component';
import { GuacamoleWindowsComponent } from './guacamole-windows/guacamole-windows.component';
import { HelpComponent } from './help/help.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MicrosoftComponent } from './microsoft/microsoft.component';
import { MysqlComponent } from './mysql/mysql.component';
import { NosqlComponent } from './nosql/nosql.component';
import { OracleComponent } from './oracle/oracle.component';
import { PrintjobsComponent } from './printjobs/printjobs.component';
import { PrintquotaComponent } from './printquota/printquota.component';
import { PrintsubmitComponent } from './printsubmit/printsubmit.component';
import { ResearchlabComponent } from './researchlab/researchlab.component';
import { SshComponent } from './ssh/ssh.component';
import { VdiComponent } from './vdi/vdi.component';
import { VpnComponent } from './vpn/vpn.component';
import { WebinfoComponent } from './webinfo/webinfo.component';
import { WeburlComponent } from './weburl/weburl.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';

//const routes: Routes = [];
const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      // {path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
      {path: 'members', component: MemberListComponent},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'webinfo', component: WebinfoComponent},
      {path: 'weburl', component: WeburlComponent},
      {path: 'docker', component: DockerComponent},
      {path: 'guacamole-linux', component: GuacamoleLinuxComponent},
      {path: 'guacamole-windows', component: GuacamoleWindowsComponent},
      {path: 'vdi', component: VdiComponent},
      {path: 'researchlab', component: ResearchlabComponent},
      {path: 'vpn', component: VpnComponent},
      {path: 'printjobs', component: PrintjobsComponent},
      {path: 'printquota', component: PrintquotaComponent},
      {path: 'printsubmit', component: PrintsubmitComponent},
      {path: 'cwps', component: CwpsComponent},
      {path: 'mysql', component: MysqlComponent},
      {path: 'oracle', component: OracleComponent},
      {path: 'nosql', component: NosqlComponent},
      {path: 'ssh', component: SshComponent},
      {path: 'microsoft', component: MicrosoftComponent},
      {path: 'help', component: HelpComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
    ]
  },
  {path: 'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
