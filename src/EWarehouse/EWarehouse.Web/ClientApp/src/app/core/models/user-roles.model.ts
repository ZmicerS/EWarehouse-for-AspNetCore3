import { AssignedRole } from './assigned-role.model';

export interface UserRoles {
  id?: number;
  userName: string;
  assignedRoles: AssignedRole[]

}
