import { Role } from '../models';

export interface User {
  id?: number;
  userName: string;
  assignedRoles: Role[];
}
