import { formatDate, formatMoney } from '@/shared/lib/formatters';

import type { TimeEntryType } from '@/entities/time-entry/model/time-entry.schema';

interface Props {
  entries: TimeEntryType[];
  onEdit: (entry: TimeEntryType) => void;
  onDelete: (id: string) => void;
}

export const TimesheetTable = ({ entries, onEdit, onDelete }: Props) => {
  const totalHours = entries.reduce((acc, x) => acc + x.hours, 0);
  const totalCost = entries.reduce((acc, x) => acc + x.hours * x.rate, 0);

  return (
    <table border={1} cellPadding={8} style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr>
          <th>Дата</th>
          <th>Сотрудник</th>
          <th>Проект</th>
          <th>Часы</th>
          <th>Ставка</th>
          <th>Стоимость</th>
          <th>Комментарий</th>
          <th>Переработка</th>
          <th>Действия</th>
        </tr>
      </thead>
      <tbody>
        {entries.map((e) => (
          <tr key={e.id}>
            <td>{formatDate(e.timeSheetDate)}</td>
            <td>{e.employeeName}</td>
            <td>{e.projectNumber}</td>
            <td>{e.hours} ч.</td>
            <td>{formatMoney(e.rate)}</td>
            <td>{formatMoney(e.price)}</td>
            <td>{e.comment || '-'}</td>
            <td>{e.isOvertime ? '⚠️ Да' : 'Нет'}</td>
            <td>
              <button onClick={() => onEdit(e)}>✏️</button>
              <button onClick={() => onDelete(e.id)}>🗑️</button>
            </td>
          </tr>
        ))}
      </tbody>
      <tfoot>
        <tr style={{ fontWeight: 'bold', background: '#f0f0f0' }}>
          <td colSpan={3}>ИТОГО:</td>
          <td>{totalHours} ч.</td>
          <td>-</td>
          <td>{formatMoney(totalCost)}</td>
          <td colSpan={3}>-</td>
        </tr>
      </tfoot>
    </table>
  );
};